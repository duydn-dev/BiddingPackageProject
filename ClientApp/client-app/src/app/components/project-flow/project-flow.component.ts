import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
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
  activeIndex: number = 0;
  currentState: string;
  isShowModal:boolean = false;
  documentForm:FormGroup;
  isSubmit:boolean = false;
  dropdownDocument:any = [];
  get form() { return this.documentForm.controls; }

  constructor(
    private _fb: FormBuilder,
    private route: ActivatedRoute,
    private _biddingService: BiddingService,
    private _documentService: DocumentService,
    private _flowService: ProjectFlowService
  ) { }

  async ngOnInit(): Promise<void> {
    this.projectId = this.route.snapshot.paramMap.get('projectId');
    //this.getDocumemtByPackage();
    this.documentForm = this._fb.group({
      projectFlowId: this._fb.control(null),
      documentNumber: this._fb.control(null, [Validators.required]),
      projectDate: this._fb.control(null, [Validators.required]),
      promulgateUnit: this._fb.control(null, [Validators.required]),
      documentAbstract: this._fb.control(null),
      signer: this._fb.control(null, [Validators.required]),
      regulationDocument: this._fb.control(null, [Validators.required]),
      fileUrl: this._fb.control(null),
      note: this._fb.control(null),
      status: this._fb.control(null, [Validators.required]),
      biddingPackageId: this._fb.control(null),
      projectId: this._fb.control(null),
      documentId: this._fb.control(null, [Validators.required])
    });
    await this.getPackageByProjectId();
    await this.projectCurrentState();
  }
  openCreatePackageForm() {

  }
  openCreateDocumentForm(){
    this.isShowModal = true;
  }
  async getPackageByProjectId() {
    const response = await this._biddingService.getPackageProjectId(this.projectId).toPromise();
    if (response.success) {
      const packageConverted = response.responseData.map(n => ({
        biddingPackageId: n.biddingPackageId,
        label: n.biddingPackageName,
        order: n.order,
        command: (event: any) => {
          this.activeIndex = n.order;
        }
      }))
      this.packages = packageConverted;
    }
  }
  // async getDocumemtByPackage(){
  //   const response = await this._documentService.getDropDownByPackage(this.currentState).toPromise();
  //   this.dropdownDocument = response.responseData;
  // }
  async projectCurrentState(){
    const response = await this._flowService.projectCurrentState(this.projectId).toPromise();
    // this.dropdownDocument = response.responseData;
    let mappedModel = response.responseData.map(n => {
      const temp = this.packages.find(g => g.biddingPackageId === n.biddingPackageId);
      if(temp){
        return {
          biddingPackageId: n.biddingPackageId,
          currentNumberDocument: n.currentNumberDocument,
          totalDocument: n.totalDocument,
          order: temp.order
        }
      }
    })
    console.log(mappedModel);
  }
  saveDocument(){
    this.isSubmit = true;
    if (this.documentForm.invalid) {
      return;
    }
    console.log(this.documentForm.value);
  }
}
