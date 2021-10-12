import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseService } from '../base/base-service.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private _baseService: BaseService
    ) {
  }

  login(user:any){
    return this._baseService.post('api/user/login',{...user});
  }
  getCurrentUser(){
    return localStorage.getItem('user');
  }
  getRoles(userId: any){
    return this._baseService.get("api/role/get-user-role",userId);
  }
  getPositionsDropdown(){
    return this._baseService.get("api/position/dropdown");
  }
  getUsers(params:any){
    return this._baseService.getWithQuery("api/user", "request", JSON.stringify(params));
  }
  getUserById(userId){
    return this._baseService.get("api/user",userId);
  }
  createUser(data:any){
    return this._baseService.post("api/user/create",data);
  }
  editUser(data:any){
    return this._baseService.put("api/user/update", data.userId, data);
  }
  removeUser(userId:any){
    return this._baseService.delete("api/user/delete", userId);
  }
  uploadAvatar(file:any) :Observable<any>{
    const formData = new FormData(); 
    formData.append('file', file);
    return this._baseService.uploadFile("api/user/upload-avatar",formData)
  }
}
