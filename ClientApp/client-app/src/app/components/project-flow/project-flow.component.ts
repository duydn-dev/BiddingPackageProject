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
  documentImported: any = [];
  isEnableCreate:boolean = true;
  @ViewChild('inputFile') input: ElementRef;
  get form() { return this.documentForm.controls; }

  constructor(
    private _fb: FormBuilder,
    private route: ActivatedRoute,
    private _messageService: MessageService,
    private _biddingService: BiddingService,
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
      documentId: this._fb.control(null, [Validators.required])
    });
    await this.initData();
  }
  openCreateDocumentForm(flowId:any = null){
    this.documentForm.clearValidators();
    this.documentForm.reset();
    this.isShowModal = true;
    this.file = null;
    this.input.nativeElement.value = null;
    if(flowId){
      this._flowService.getFlowById(flowId).subscribe(response => {
        if(response.success){
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
            documentId: response.responseData.documentId
          });
        }
      })
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
    this.isEnableCreate = (flow.currentNumberDocument == flow.totalDocument)
  }
  // async getCurrentPackage(){
  //   const response = await this._flowService.currentPackage(this.projectId).toPromise();
  //   if(response.success){
  //     this.currentPackage = response.responseData;
  //   }
  // }
  async getTotalAndCompleteDocument(){
    const response = await this._flowService.projectCurrentState(this.projectId).toPromise();
    if (response.success) {
      this.packages = response.responseData;
      console.log(this.packages);
    }
  }
  // async getPackageByProjectId() {
  //   const response = await this._biddingService.getPackageProjectId(this.projectId).toPromise();
  //   if (response.success) {
  //     const packageConverted = response.responseData.map(n => ({
  //       biddingPackageId: n.biddingPackageId,
  //       label: n.biddingPackageName,
  //       order: n.order,
  //     }))
  //     return packageConverted;
  //   }
  // }

  // async projectCurrentState(packages){
  //   const response = await this._flowService.projectCurrentState(this.projectId).toPromise();
  //   let mappedModel = response.responseData.map(n => {
  //     const temp = packages.find(g => g.biddingPackageId === n.biddingPackageId);
  //     if(temp){
  //       return {
  //         biddingPackageId: n.biddingPackageId,
  //         currentNumberDocument: n.currentNumberDocument,
  //         totalDocument: n.totalDocument,
  //         order: temp.order,
  //         label: temp.label,
  //         command: async (event: any) => {
  //           this.activeIndex = (temp.order - 1);
  //           this.currentPackage = n.biddingPackageId;
  //           this.enableCreate();
  //           await this.getFlows();
  //         }
  //       }
  //     }
  //   }).sort(function (a, b) {
  //     return a.order - b.order;
  //   })
  //   this.packages = mappedModel;
  //   const currentOrder = this.packages.find(n => n.biddingPackageId === this.currentPackage)
  //   this.activeIndex = !currentOrder ? 0 : (currentOrder.order - 1)
  // }
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
    //await this.getCurrentPackage();
    //await this.projectCurrentState(await this.getPackageByProjectId());
    //await this.getFlows();
    //await this.getDropDownDocument();
    //this.enableCreate();
    await this.getTotalAndCompleteDocument();
  }
}
