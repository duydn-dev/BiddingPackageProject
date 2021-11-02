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
  async buildSelected(){
    const response = await this.documentService.getSettingSelected(this.projectId).toPromise();
    if(response.success){
      if(response.responseData.length > 0){
        console.log(this.packageDocument)
        this.packageDocumentSelected.push(...response.responseData.map(n => n.documentId))
        console.log(this.packageDocumentSelected);
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
  buildSaveModel(){
    let arr = this.packageDocument.filter(n => this.packageDocumentSelected.includes(n.data));
    const saveModel = arr.map(n => ({
        biddingPackageId: n.data,
        documents: n.children.map(n => ({documentId: n.data, order : parseInt(n.order) }))
    }))
    return saveModel;
  }
  save(){
    const model = this.buildSaveModel();
    this.documentService.saveDocumentSetting(this.projectId, model).subscribe(response => {
      if(response.success){
        this._messageService.add({severity:'success', summary:'Thành công', detail:'chỉnh sửa người dùng thành công !'});
      }
      else{
        this._messageService.add({severity:'error', summary:'Lỗi', detail: response.message});
      }
    })
  }
  async ngOnInit(): Promise<void> {
    this.projectId = this.route.snapshot.paramMap.get('projectId');
    await this.getSettings();
    await this.buildSelected();
  }
}
