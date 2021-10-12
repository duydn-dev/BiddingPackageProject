import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor(
  ) { }
  checkLogin(){
    const data:any = JSON.parse(localStorage.getItem("user"));
    if(!data || !data.user)
      return false;
    else if(new Date(data.user.expire) < new Date()){
      return false;
    }
    return true;
  }
}
