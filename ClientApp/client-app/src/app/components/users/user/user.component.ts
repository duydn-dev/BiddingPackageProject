import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { UserService } from 'src/app/services/user/user.service';
import { ApiConfig } from '../../../confis/api-config';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  // variable 
  search: any = {
    textSearch: "",
    pageSize: 20,
    pageIndex: 1
  }
  users: any[] = [];
  totalData: number = 0;
  isShowModal:boolean = false;
  isSubmit:boolean = false;
  userForm: FormGroup;
  userId: string;
  userStatus:any[] = [];
  positions:any[] = [];
  avatar:any;
  previewAvatar:string;
  fullAvatarPath: string;
  get form() { return this.userForm.controls; }

  // init funtion
  constructor(
    private _fb: FormBuilder,
    private confirmationService: ConfirmationService,
    private _messageService: MessageService,
    private _userService: UserService
  ) { }

  ngOnInit(): void {
    this.userStatus = ApiConfig.userType;
    this.userForm = this._fb.group({
      userId: this._fb.control(null),
      userName: this._fb.control(null, [Validators.required]),
      fullName: this._fb.control(null),
      avatar: this._fb.control(null),
      passWord: this._fb.control(null, [Validators.required]),
      email: this._fb.control(null, [Validators.required, Validators.email]),
      numberPhone: this._fb.control(null),
      address: this._fb.control(null),
      status: this._fb.control(null, [Validators.required]),
      userPositionId: this._fb.control(null, [Validators.required]),
      createdBy: this._fb.control(null),
      createdDate: this._fb.control(null),
      modifiedBy: this._fb.control(null),
      modifiedDate: this._fb.control(null)
    });
    this.getPositions();
    this.getFilter();
  }

  // user-definded method
  getFilter(){
    this._userService.getUsers(this.search).subscribe(response => {
      this.users = response.responseData.data;
      this.totalData = response.responseData.totalData;
    })
  }
  getPositions(){
    this._userService.getPositionsDropdown().subscribe(response => {
      this.positions = response.responseData;
    })
  }
  searchData(){
    this.search.pageIndex = 1;
    this.getFilter();
  }
  onPageChange(event:any){
    this.search.pageIndex = (event.page + 1);
    this.getFilter();
  }
  openEditForm(userId = null){
    this.previewAvatar = null;
    this.avatar = null;
    this.userId = userId;
    this.previewAvatar = null;
    this.fullAvatarPath = null;
    this.userForm.reset();
    if(!userId){
      this.isShowModal = true;
    }
    else{
      this._userService.getUserById(userId).subscribe(response => {
        this.previewAvatar = response.responseData.avatar;
        this.fullAvatarPath = ApiConfig.apiUrl + "/" + this.previewAvatar;
        this.userForm.patchValue({...response.responseData});
        this.isShowModal = true;
      })
    }
  }
  openDeleteForm(userId){
    this.confirmationService.confirm({
      message: 'Bạn có chắc chắn muốn xóa ?',
      header: '',
      accept: () => {
         this._userService.removeUser(userId).subscribe(
          response => {
            if(!response.responseData){
              this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: response.message });
            }
          })
      },
      reject: () => {
      }
    });
  }
  onUploadAvatar(event){
    if(event.currentFiles.length > 0){
      this.avatar = event.currentFiles[0];
      this._userService.uploadAvatar(this.avatar).subscribe(response => {
        this.previewAvatar = response.responseData;
        this.fullAvatarPath = ApiConfig.apiUrl + "/" + this.previewAvatar;
      })
    }
  }
  save(){
    this.isSubmit = true;
    if (this.userForm.invalid) {
      return;
    }
    this.userForm.get('avatar').patchValue(this.previewAvatar);
    if(this.userId){
      this._userService.editUser(this.userForm.value).subscribe(response => {
        if(response.success){
          this.getFilter();
          this._messageService.add({severity:'success', summary:'Thành công', detail:'chỉnh sửa người dùng thành công !'});
          this.isShowModal = false;
        }
        else{
          this._messageService.add({severity:'error', summary:'Lỗi', detail: response.message});
        }
      })
    }
    else{
      this._userService.createUser(this.userForm.value).subscribe(response => {
        if(response.success){
          this.getFilter();
          this._messageService.add({severity:'success', summary:'Thành công', detail:'Thêm mới người dùng thành công !'});
          this.isShowModal = false;
        }
        else{
          this._messageService.add({severity:'error', summary:'Lỗi', detail: response.message});
        }
      })
    }
  }
}
