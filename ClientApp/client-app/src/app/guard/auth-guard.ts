import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { CommonService } from '../services/base/common.service';
import { Router } from '@angular/router';
import { State } from "../ngrx";
import * as userActions from '../ngrx/actions/login.action'; 
import { Store } from '@ngrx/store';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private _commonService: CommonService,
    private _router: Router,
    private _store : Store<State>,
    private _messageService: MessageService,
  ) {
  }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      const isLogin = this._commonService.checkLogin();
      if(isLogin){
        return true;
      }
      else{
        this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: "thời gian hoạt động đã hết, hệ thống sẽ tự chuyển hướng về trang đăng nhập !" });
          setTimeout(() => {
              this._store.dispatch(userActions.logout());
              this._router.navigate(["login"]);
          }, 2000);
        return false;
      }
      //return true;
  }
}
