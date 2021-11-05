import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BiddingService } from 'src/app/services/bidding-package/bidding-package.service';
import { ProjectService } from 'src/app/services/project/project.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit {
  search: any = {
    textSearch: "",
    pageSize: 20,
    pageIndex: 1
  }

  projectId:any;
  projects:any[] = [];
  totalData: number= 0;
  isShowModal:boolean = false;
  isSubmit:boolean = false;
  projectForm: FormGroup;
  get form() { return this.projectForm.controls; }

  // package
  packageSearch:any = {
    textSearch: "",
    pageSize: 20,
    pageIndex: 1
  }
  packages:any = [];
  totalPackage: number = 0;
  packageSelected: any = [];
  constructor(
    private _fb: FormBuilder,
    private _projectService: ProjectService,
    private confirmationService: ConfirmationService,
    private _messageService: MessageService,
    private _packageService: BiddingService
  ) { }

  ngOnInit(): void {
    this.projectForm = this._fb.group({
      projectId: this._fb.control(null),
      projectName: this._fb.control(null, [Validators.required]),
      projectDate: this._fb.control(null, [Validators.required]),
      note: this._fb.control(null)
    })

    this.getFilter();
    this.getPackageFilter();
  }
  getFilter(){
    this._projectService.getFilter(this.search).subscribe(response => {
      if(response.success){
        this.projects = response.responseData.data;
        this.totalData = response.responseData.totalData;
      }
    })
  }

  searchData(){
    this.search.pageIndex = 1;
    this.getFilter();
  }
  onPageChange(event){
    this.search.pageIndex = (event.page + 1);
    this.getFilter();
  }
  exportExcel(projectId:any){
    var downloadURL = `${environment.apiUrl}/api/project/export-excel/${projectId}`;
    window.open(downloadURL);
  }
  openEditForm(projectId:any = null){
    this.projectForm.reset();
    this.projectId = projectId;
    this.packageSelected = [];
    if(projectId){
      this._projectService.getById(projectId).subscribe(response => {
        if(response.success){
          this.projectForm.patchValue({
            note: response.responseData.note,
            projectDate: new Date(response.responseData.projectDate),
            projectId: response.responseData.projectId,
            projectName: response.responseData.projectName
          });
          if(response.responseData.biddingPackageDtos && response.responseData.biddingPackageDtos.length > 0){
            this.packageSelected = response.responseData.biddingPackageDtos
          }
          this.isShowModal = true;
        }
      })
    }
    else{
      this.isShowModal = true;
    }
  }
  openDeleteForm(projectId:any){
    this.confirmationService.confirm({
      message: 'Bạn có chắc chắn muốn xóa ?',
      header: '',
      accept: () => {
         this._projectService.delete(projectId).subscribe(
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
    if (this.projectForm.invalid) {
      return;
    }
    if(this.packageSelected.length <= 0){
      this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: "Dự án phải bao gồm gói thầu !" });
      return;
    }
    const data = this.projectForm.value;
    data.biddingPackageDtos = this.packageSelected;
    if(!this.projectForm.get("projectId").value){
      this._projectService.create(data).subscribe(response => {
        if(response.success){
          this.getFilter();
          this._messageService.add({ severity: 'success', summary: 'Thành công', detail: "Thêm mới thành công !" });
          this.isShowModal = false;
        }
        else{
          this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: response.message });
        }
      })
    }
    else{
      this._projectService.update(this.projectForm.get("projectId").value, data).subscribe(response => {
          if(response.success){
            this.getFilter();
            this._messageService.add({ severity: 'success', summary: 'Thành công', detail: "Chỉnh sửa thành công !" });
            this.isShowModal = false;
          }
          else{
            this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: response.message });
          }
      })
    }
  }


  // package
  getPackageFilter(){
    this._packageService.getFilter(this.packageSearch).subscribe(response => {
      if(response.success){
        this.packages = response.responseData.data;
        this.totalPackage = response.responseData.totalData;
      }
    })
  }
  onRowSelect(event){
    const packageLength = this.packageSelected.length;
    let i = this.packageSelected.findIndex(n => n.biddingPackageId === event.data.biddingPackageId);
    this.packageSelected[i].order = packageLength;
  }
  onRowUnselect(event){
    event.data.order = null;
    this.packageSelected.forEach((e, i) => {
      e.order = (i + 1)
    });
  }
  packagePageChange(event){
    this.packageSearch.pageIndex = (event.page + 1);
    this.getPackageFilter();
  }
  findOrder(packageItem:any){
    const order = this.packageSelected.find(n => n.biddingPackageId == packageItem.biddingPackageId);
    return order?.order;
  }
}
