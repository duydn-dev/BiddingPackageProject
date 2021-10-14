import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ProjectService } from 'src/app/services/project/project.service';

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
  constructor(
    private _fb: FormBuilder,
    private _projectService: ProjectService,
    private confirmationService: ConfirmationService,
    private _messageService: MessageService,
  ) { }

  ngOnInit(): void {
    this.projectForm = this._fb.group({
      projectId: this._fb.control(null),
      projectName: this._fb.control(null, [Validators.required]),
      projectDate: this._fb.control(null),
      note: this._fb.control(null),
      biddingPackageDtos: this._fb.array([])
    })

    this.getFilter();
  }
  getFilter(){
    this._projectService.getFilter(this.search).subscribe(response => {
      if(response.success){
        console.log(response);
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
  openEditForm(projectId:any = null){
    this.projectId = projectId;
    this.isShowModal = true;
  }
  openDeleteForm(projectId:any){
    this.confirmationService.confirm({
      message: 'Bạn có chắc chắn muốn xóa ?',
      header: '',
      accept: () => {
         this._projectService.delete(projectId).subscribe(
          response => {
            if(response.responseData){
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
  }
}
