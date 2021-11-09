import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BiddingService } from 'src/app/services/bidding-package/bidding-package.service';
import { DocumentService } from 'src/app/services/document/document.service';
import { ProjectFlowService } from 'src/app/services/project-flow/project-flow.service';
import { environment } from 'src/environments/environment';

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
  file:any = null;
  document:any = [];
  flowSythentic:any = [];
  documentImported: any = [];
  isEnableCreate:boolean = true;
  isSynthetic:boolean = false;
  @ViewChild('inputFile') input: ElementRef;
  get form() { return this.documentForm.controls; }

  constructor(
    private _fb: FormBuilder,
    private route: ActivatedRoute,
    private _messageService: MessageService,
    private _documentService: DocumentService,
    private _flowService: ProjectFlowService,
    private _confirmationService: ConfirmationService,
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
      documentId: this._fb.control(null, [Validators.required]),
      isMainDocument: this._fb.control(false),
    });
    await this.initData();
  }
  async openCreateDocumentForm(flow:any, isSynthetic:boolean){
    const flowId:any = (flow) ? flow.projectFlowId: null;
    this.isSynthetic = isSynthetic;
    this.currentPackage = flow ? flow.biddingPackageId: this.currentPackage;
    if(isSynthetic){
      await this.getSynthetic();
    }
    else{
      await this.getDropDownDocument();
    }
    this.documentForm.clearValidators();
    this.documentForm.reset();
    if(flowId){
      const response = await this._flowService.getFlowById(flowId).toPromise();
      if(response.success){
        this.isShowModal = true;
        this.file = null;
        this.isSubmit = false;
        this.input.nativeElement.value = null;
        this.file = {
          name: this.getFileUrlv2(response.responseData.fileUrl)
        };
        this.documentForm.patchValue({
          projectFlowId: response.responseData.projectFlowId,
          documentNumber: response.responseData.documentNumber,
          projectDate: new Date(response.responseData.projectDate),
          promulgateUnit: response.responseData.documentAbstract,
          documentAbstract: response.responseData.documentAbstract,
          signer: response.responseData.signer,
          regulationDocument: response.responseData.regulationDocument,
          note: response.responseData.note,
          status: response.responseData.status,
          biddingPackageId: response.responseData.biddingPackageId,
          projectId: response.responseData.projectId,
          documentId: response.responseData.documentId,
          isMainDocument: response.responseData.isMainDocument,
        });
      }
    }
    else{
      this.isShowModal = true;
      this.file = null;
      this.isSubmit = false;
      this.input.nativeElement.value = null;
    }
  }
  getFileUrl(url){
    return url.replace(/\d{14}./gm, ".");
  }
  getFileUrlv2(url){
    let str = url.replace(/\d{14}./gm, ".");
    return str.replace(/Uploads\\/gm, "");
  }
  downloadFile(projectFlowId){
    var downloadURL = `${environment.apiUrl}/api/projectflow/download/${projectFlowId}`;
    window.open(downloadURL); 
  }
  async getDropDownDocument(){
    const request = {
      packageId: this.currentPackage,
      projectId: this.projectId
    }
    const response = await this._documentService.getDropDownByPackage(request).toPromise();
    if(response.success){
      response.responseData.unshift({
        documentId: null,
        documentName: '-- Chọn văn bản --'
      })
      if(response.otherData){
        this.documentImported = response.otherData;
        this.dropdownDocument = response.responseData.map(n => {
          const str = this.isDisabledOption(n);
          n.disabled = (str == 'disabled');
          return n;
        });
      }
    }
  }
  async getSynthetic(){
    const response = await this._documentService.getSynthetic().toPromise();
    if(response.success){
      response.responseData.unshift({
        documentId: null,
        documentName: '-- Chọn văn bản --'
      })
      this.dropdownDocument = response.responseData;
    }
  }
  isDisabledOption(option){
    if(option.documentId){
      const i = this.documentImported.findIndex(n => n == option.documentId);
      return i === -1 ? "": "disabled";
    }
    return "";
  }
  openDeletePop(projectFlowId:any){
    this._confirmationService.confirm({
      message: 'Bạn có chắc chắn muốn xóa ?',
      header: '',
      accept: () => {
         this._flowService.delete(projectFlowId).subscribe(
          response => {
            if(response.responseData){
              this.initData();
              this._messageService.add({ severity: 'success', summary: 'Thành công !', detail: 'Xóa thành công !' });
            }
            else {
              this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: response.message });
            }
          })
      },
      reject: () => {
      }
    });
  }
  onFileChange(event){
    if(event.target.files.length > 0)
      this.file = event.target.files[0];
  }
  enableCreate(){
    const flow = this.packages.find(n => n.biddingPackageId == this.currentPackage);
    this.isEnableCreate = ((flow.currentDocumentCount == flow.documentCount) && (flow.documentCount == 0))
  }
  async getFlowSynthetic(){
    const response = await this._flowService.getFlowSynthetic(this.projectId).toPromise();
    this.flowSythentic = response.responseData;
  }
  async getTotalAndCompleteDocument(){
    const response = await this._flowService.projectCurrentState(this.projectId).toPromise();
    if (response.success) {
      this.packages = response.responseData.map(n => ({
        biddingPackageId: n.biddingPackageId,
        currentDocumentCount: n.currentDocumentCount,
        documentCount: n.documentCount,
        order: n.order,
        label: n.biddingPackageName,
        command: async (event: any) => {
          this.activeIndex = (n.order - 1);
          this.currentPackage = n.biddingPackageId;
          this.enableCreate();
          await this.getDropDownDocument();
          await this.getFlows();
        }
      }));
      this.currentPackage = response.otherData.currentPackageId;
      this.activeIndex = (response.otherData.order - 1);
      this.enableCreate();
      await this.getFlows();
    }
  }
  async setActiveIndex(pkg:any){
    this.activeIndex = (pkg.order - 1);
    this.currentPackage = pkg.biddingPackageId;
    this.enableCreate();
    await this.getDropDownDocument();
    await this.getFlows();
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
    request.biddingPackageId = this.isSynthetic ? null: this.currentPackage;

    if(this.documentForm.get('projectFlowId').value){
      const response = await this._flowService.updateFlow(this.file, request).toPromise();
      if(response.success){
        await this.getFlows();
        this.isSubmit = false;
        this.isShowModal = false;
        this._messageService.add({ severity: 'success', summary: 'Thành công !', detail: 'Cập nhật thành công !' });
      }
      else{
        this._messageService.add({ severity: 'error', summary: 'Lỗi !', detail: response.message });
      }
    }
    else{
      const response = await this._flowService.createFlow(this.file, request).toPromise();
      if(response.success){
        await this.initData();
        this.isSubmit = false;
        this.isShowModal = false;
        this._messageService.add({ severity: 'success', summary: 'Thành công !', detail: 'Thêm mới thành công !' });
      }
      else{
        this._messageService.add({ severity: 'error', summary: 'Lỗi !', detail: response.message });
      }
    }
  }
  async initData(){
    await this.getTotalAndCompleteDocument();
    await this.getFlowSynthetic();
  }
}
