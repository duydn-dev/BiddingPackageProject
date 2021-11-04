import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MessageService, TreeNode } from 'primeng/api';
import { DocumentService } from 'src/app/services/document/document.service';

@Component({
  selector: 'app-document-setting',
  templateUrl: './document-setting.component.html',
  styleUrls: ['./document-setting.component.css']
})
export class DocumentSettingComponent implements OnInit {
  projectId: any;
  packageDocument:any = [];
  packageDocumentSelected:any = [];
  isInitDocument:boolean = true;
  documentSelectedOrder:any = [];
  headers:any = [
    { field: 'label', header: 'Tên cấu hình' },
  ]
  constructor(
    private route: ActivatedRoute,
    private documentService : DocumentService,
    private _messageService: MessageService,
  ) { }
  
  async getSettings(){
    const response = await this.documentService.getSettingDocument(this.projectId).toPromise();
    if(response.success){
      let parents = [];
      parents = response.responseData.filter(n => typeof n?.documents !== undefined);
      if(parents.length > 0){
        parents = parents.map(n => ({data: n.biddingPackageId, label: n.biddingPackageName, isParent: true , expanded: true, children: this.getChilds(n.documents)}))
      }
      this.packageDocument = parents;
    }
  }
  getChilds(documents){
    return (documents && documents.length > 0) 
    ? documents.map(n => ({data: n.documentId, label: n.documentName, isCommon: n.isCommon, order: n.order}))
    : undefined
  }
  getOrder(data){
    if(data && this.documentSelectedOrder.length > 0){
      const doc = this.documentSelectedOrder.find(n => n.documentId == data.data);
      data.order = doc ? doc.order: null;
      return doc ? doc.order: null;
    }
    return null;
  }
  changeOrder(event, node){
    if(!this.isInitDocument){
      const exits = this.documentSelectedOrder.find(n => n.documentId == node.data);
      if(exits){
        exits.order = parseInt(event);
      }
      else{
        this.documentSelectedOrder.push({
          biddingPackageId: node.parent.data,
          documentId: node.data,
          documentSettingId: null,
          order: parseInt(event),
          projectId: this.projectId,
        })
      }
    }
  } 
  async buildSelected(){
    const response = await this.documentService.getSettingSelected(this.projectId).toPromise();
    if(response.success){
      if(response.responseData.length > 0){
        this.isInitDocument = false;
        this.documentSelectedOrder = Array.from(response.responseData);
        console.log('this.documentSelectedOrder', this.documentSelectedOrder)
        this.packageDocumentSelected = [];
        this.packageDocumentSelected.push(...response.responseData.map(n => n.documentId))
      }
      else{
        this.packageDocumentSelected = [];
        let data = this.packageDocument.filter(n => n.isParent);
        this.packageDocumentSelected = data.map(n => n.data);
        let childArr = [];
        this.packageDocument.forEach(element => {
          if(element.children.length > 0){
            childArr.push(...element.children.filter(n => n.isCommon))
          }
        });
        this.packageDocumentSelected.push(...childArr.map(n => n.data))
      }
    }
  }
  checkboxChange(event, node){
    node.isDisabled = !event.checked;
    if(!event){
      const i = this.packageDocumentSelected.findIndex(n => n.data == node.data);
      this.packageDocumentSelected.splice(i, 1);
    }
  }
  buildChildSaveModel(biddingPackageId){
    const currentSelectedPackages = this.documentSelectedOrder.filter(n => n.biddingPackageId == biddingPackageId).map(n => ({ documentId: n.documentId, order: n.order}));
    const packages = this.packageDocument.find(n => n.data == biddingPackageId);
    const childs = [];
    packages.children.map(n => {
      const document = currentSelectedPackages.find(g => g.documentId == n.data)
      if(document){
        childs.push(document);
      }
    })
    return childs;
  }
  buildSaveModel(){
    if(this.isInitDocument){
      let arr = this.packageDocument.filter(n => this.packageDocumentSelected.includes(n.data));
      const ids = arr.map(g => g.data);
      const documentIds = this.packageDocumentSelected.filter(n => !ids.includes(n));
  
      const saveModel = arr.map(n => ({
          biddingPackageId: n.data,
          documents: n.children.filter(g => documentIds.includes(g.data)).map(g => ({documentId: g.data, order : parseInt(g.order) }))
      }))
      return saveModel;
    }
    else{
      const saveModel = this.packageDocument.map(n => ({
          biddingPackageId : n.data,
          documents : this.buildChildSaveModel(n.data)
      }));
      return saveModel;
    }
  }
  async save(){
    const model = this.buildSaveModel();
    const response = await this.documentService.saveDocumentSetting(this.projectId, model).toPromise();
    if(response.success){
      await this.getSettings();
      await this.buildSelected();
      this._messageService.add({severity:'success', summary:'Thành công', detail:'cấu hình văn bản thành công !'});
    }
    else{
      this._messageService.add({severity:'error', summary:'Lỗi', detail: response.message});
    }
  }
  async ngOnInit(): Promise<void> {
    this.projectId = this.route.snapshot.paramMap.get('projectId');
    await this.getSettings();
    await this.buildSelected();
  }
}
