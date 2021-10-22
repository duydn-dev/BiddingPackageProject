import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import { BiddingService } from 'src/app/services/bidding-package/bidding-package.service';
import { DocumentService } from 'src/app/services/document/document.service';
import { ProjectFlowService } from 'src/app/services/project-flow/project-flow.service';

@Component({
  selector: 'app-project-flow',
  templateUrl: './project-flow.component.html',
  styleUrls: ['./project-flow.component.css']
})
export class ProjectFlowComponent implements OnInit {
  projectId: any;
  packages: any = [];
  currentPackage: any;
  activeIndex: number = 0;
  currentState: string;
  isShowModal:boolean = false;
  documentForm:FormGroup;
  isSubmit:boolean = false;
  dropdownDocument:any = [];
  file:any = {};
  document:any = [];
  get form() { return this.documentForm.controls; }

  constructor(
    private _fb: FormBuilder,
    private route: ActivatedRoute,
    private _messageService: MessageService,
    private _biddingService: BiddingService,
    private _documentService: DocumentService,
    private _flowService: ProjectFlowService
  ) { }

  async ngOnInit(): Promise<void> {
    this.projectId = this.route.snapshot.paramMap.get('projectId');
    this.documentForm = this._fb.group({
      projectFlowId: this._fb.control(null),
      documentNumber: this._fb.control(null, [Validators.required]),
      projectDate: this._fb.control(null, [Validators.required]),
      promulgateUnit: this._fb.control(null, [Validators.required]),
      documentAbstract: this._fb.control(null),
      signer: this._fb.control(null, [Validators.required]),
      regulationDocument: this._fb.control(null, [Validators.required]),
      note: this._fb.control(null),
      status: this._fb.control(null, [Validators.required]),
      biddingPackageId: this._fb.control(null),
      projectId: this._fb.control(null),
      documentId: this._fb.control(null, [Validators.required])
    });
    await this.initData();
  }
  openCreatePackageForm() {
    
  }
  openCreateDocumentForm(){
    this.documentForm.reset();
    this.isShowModal = true;
  }
  async getDropDownDocument(){
    const response = await this._documentService.getDropDownByPackage(this.currentPackage).toPromise();
    if(response.success){
      this.dropdownDocument = response.responseData;
      this.dropdownDocument.unshift({
        documentId: null,
        documentName: '-- Chọn văn bản --'
      })
    }
  }
  onFileChange(event){
    if(event.target.files.length > 0)
      this.file = event.target.files[0];
  }
  async getCurrentPackage(){
    const response = await this._flowService.currentPackage(this.projectId).toPromise();
    if(response.success){
      console.log(response.responseData);
      this.currentPackage = response.responseData;
    }
  }
  async getPackageByProjectId() {
    const response = await this._biddingService.getPackageProjectId(this.projectId).toPromise();
    if (response.success) {
      const packageConverted = response.responseData.map(n => ({
        biddingPackageId: n.biddingPackageId,
        label: n.biddingPackageName,
        order: n.order,
      }))
      return packageConverted;
    }
  }

  async projectCurrentState(packages){
    const response = await this._flowService.projectCurrentState(this.projectId).toPromise();
    let mappedModel = response.responseData.map(n => {
      const temp = packages.find(g => g.biddingPackageId === n.biddingPackageId);
      if(temp){
        return {
          biddingPackageId: n.biddingPackageId,
          currentNumberDocument: n.currentNumberDocument,
          totalDocument: n.totalDocument,
          order: temp.order,
          label: temp.label,
          command: (event: any) => {
            this.activeIndex = (temp.order - 1);
            this.currentPackage = n.biddingPackageId;
          }
        }
      }
    }).sort(function (a, b) {
      return a.order - b.order;
    })
    this.packages = mappedModel;
    console.log(this.packages);
    const currentOrder = this.packages.find(n => n.biddingPackageId === this.currentPackage)
    this.activeIndex = (currentOrder?.order) ? 0 : currentOrder?.order - 1;
  }
  async getFlows(){
    const request = {
      projectId: this.projectId,
      biddingPackageId : this.currentPackage
    }
    const response = await this._flowService.getFlows(request).toPromise();
    this.document = response.responseData;
  }
  async saveDocument(){
    this.isSubmit = true;
    if (this.documentForm.invalid) {
      return;
    }
    const request = this.documentForm.value;
    request.projectId = this.projectId;
    request.biddingPackageId = this.currentPackage;

    if(this.documentForm.get('projectFlowId').value){
      console.log(this.documentForm.value)
    }
    else{
      const response = await this._flowService.createFlow(this.file, request).toPromise();
      if(response.success){
        await this.initData();
        this.isShowModal = false;
        this._messageService.add({ severity: 'success', summary: 'Thành công !', detail: 'Thêm mới thành công !' });
      }
      else{
        this._messageService.add({ severity: 'error', summary: 'Lỗi !', detail: 'Thêm mới thất bại !' });
      }
    }
  }
  async initData(){
    await this.getCurrentPackage();
    await this.projectCurrentState(await this.getPackageByProjectId());
    await this.getFlows();
    await this.getDropDownDocument();
  }
}
