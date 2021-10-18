import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BiddingService } from 'src/app/services/bidding-package/bidding-package.service';
import { DocumentService } from 'src/app/services/document/document.service';

@Component({
  selector: 'app-bidding-package',
  templateUrl: './bidding-package.component.html',
  styleUrls: ['./bidding-package.component.css']
})
export class BiddingPackageComponent implements OnInit {
  search: any = {
    textSearch: "",
    pageSize: 20,
    pageIndex: 1
  }
  biddingPackage:any = [];
  totalData:number = 0;
  isShowModal:boolean = false;
  biddingPackageId:any;
  packageForm: FormGroup;
  isSubmit:boolean = false;
  documents:any = [];

  // documents
  get form() { return this.packageForm.controls; }
  constructor(
    private _fb: FormBuilder,
    private confirmationService: ConfirmationService,
    private _messageService: MessageService,
    private _biddingService : BiddingService,
    private _documentService : DocumentService,
  ) { }

  ngOnInit(): void {
    this.getFilter();
    this.getDocuments();
    this.packageForm = this._fb.group({
      biddingPackageId: this._fb.control(null),
      biddingPackageName: this._fb.control(null, [Validators.required]),
      documentIds : this._fb.array([])
    })
  }
  getFilter(){
    this._biddingService.getFilter(this.search).subscribe(response => {
      if(response.success){
        this.biddingPackage = response.responseData.data;
        this.totalData = response.responseData.totalData;
      }
    })
  }
  openEditForm(biddingPackageId:any = null){
    this.packageForm.reset();
    this.biddingPackageId = biddingPackageId;
    if(biddingPackageId){
      this._biddingService.getById(biddingPackageId).subscribe(response => {
        if(response.success){
          console.log(response.responseData);
          // this.packageForm.patchValue({
          //   note: response.responseData.note,
          //   projectDate: new Date(response.responseData.projectDate),
          //   projectId: response.responseData.projectId,
          //   projectName: response.responseData.projectName
          // });
          // if(response.responseData.biddingPackageDtos && response.responseData.biddingPackageDtos.length > 0){
          //   this.packageSelected = response.responseData.biddingPackageDtos
          // }
          this.isShowModal = true;
        }
      })
    }
    else{
      this.isShowModal = true;
    }
  }
  openDeleteForm(biddingPackageId:any){
    this.confirmationService.confirm({
      message: 'Bạn có chắc chắn muốn xóa ?',
      header: '',
      accept: () => {
         this._biddingService.delete(biddingPackageId).subscribe(
          response => {
            if(response.responseData){
              this.getFilter();
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
  onSubmit(){
    this.isSubmit = true;
    if (this.packageForm.invalid) {
      return;
    }
  }
  searchData(){
    this.search.pageIndex = 1;
    this.getFilter();
  }
  onPageChange(event){
    this.search.pageIndex = (event.page + 1);
    this.getFilter();
  }
  getDocuments(){
    this._documentService.getDropDown().subscribe(response => {
      if(response.success){
        this.documents = response.responseData;
      }
    })
  }
}
