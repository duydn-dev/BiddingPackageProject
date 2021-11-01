import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BiddingService } from 'src/app/services/bidding-package/bidding-package.service';
import { DocumentService } from 'src/app/services/document/document.service';

@Component({
  selector: 'app-document',
  templateUrl: './document.component.html',
  styleUrls: ['./document.component.css']
})
export class DocumentComponent implements OnInit {
  search: any = {
    textSearch: "",
    pageSize: 20,
    pageIndex: 1
  }
  document:any = [];
  totalData:number = 0;
  isShowModal:boolean = false;
  documentId:any;
  documentForm: FormGroup;
  isSubmit:boolean = false;
  packages:any = [];

  // documents
  get form() { return this.documentForm.controls; }
  constructor(
    private _fb: FormBuilder,
    private confirmationService: ConfirmationService,
    private _messageService: MessageService,
    private _documentService : DocumentService,
    private _biddingService : BiddingService,
  ) { }

  ngOnInit(): void {
    this.getFilter();
    this.getDropdownPackage();
    this.documentForm = this._fb.group({
      documentId: this._fb.control(null),
      documentName: this._fb.control(null, [Validators.required]),
      note: this._fb.control(null),
      isCommon: this._fb.control(false),
      biddingPackageId: this._fb.control(null, [Validators.required]),
    })
  }
  getFilter(){
    this._documentService.getFilter(this.search).subscribe(response => {
      if(response.success){
        this.document = response.responseData.data;
        this.totalData = response.responseData.totalData;
      }
    })
  }
  openEditForm(documentId:any = null){
    this.documentForm.reset();
    this.documentForm.clearValidators();
    this.documentId = documentId;
    if(documentId){
      this._documentService.getById(documentId).subscribe(response => {
        if(response.success){
          this.documentForm.patchValue({
              documentId: response.responseData.documentId,
              documentName: response.responseData.documentName,
              note: response.responseData.note,
              isCommon: response.responseData.isCommon,
              biddingPackageId: response.responseData.biddingPackageId
            });
          this.isShowModal = true;
        }
      })
    }
    else{
      this.isShowModal = true;
    }
  }
  openDeleteForm(documentId:any){
    this.confirmationService.confirm({
      message: 'Bạn có chắc chắn muốn xóa ?',
      header: '',
      accept: () => {
         this._documentService.delete(documentId).subscribe(
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
    if (this.documentForm.invalid) {
      return;
    }
    if(this.documentForm.get("documentId").value){
      if(!this.documentForm.value.isCommon)
          this.documentForm.value.isCommon = false;

      this._documentService.update(this.documentForm.value.documentId, this.documentForm.value).subscribe(response => {
        if(response.success){
          this.getFilter();
          this.isShowModal = false;
          this._messageService.add({ severity: 'success', summary: 'Thành công !', detail: 'Cập nhật thành công !' });
        }
        else{
          this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: response.message });
        }
      })
    }
    else{
      this._documentService.create(this.documentForm.value).subscribe(response => {
        if(response.success){
          this.getFilter();
          this.isShowModal = false;
          this.isSubmit = false;
          this._messageService.add({ severity: 'success', summary: 'Thành công !', detail: 'Thêm mới thành công !' });
        }
        else{
          this.isSubmit = false;
          this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: response.message });
        }
      })
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
  getDropdownPackage(){
    this._biddingService.getDropdown().subscribe(response => {
      if(response.success){
        this.packages = response.responseData;
        this.packages.unshift({biddingPackageId: null, biddingPackageName: "--- Chọn gói thầu ---"})
      }
    })
  }
}