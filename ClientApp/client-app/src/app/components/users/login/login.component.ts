import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Router } from '@angular/router';

import { State } from 'src/app/ngrx/reducers/user.reducer';
import * as userActions from '../../../ngrx/actions/login.action';
import { UserService } from '../../../services/user/user.service';
import { MessageService } from 'primeng/api';
import { SpinnerService } from 'src/app/services/spinner/spinner.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup
  isSubmit:boolean = false;
  get form() { return this.loginForm.controls; }


  constructor(
    private _fb: FormBuilder,
    private store: Store<State>,
    private _router: Router,
    private _userService: UserService,
    private _messageService: MessageService,
    private _spinnerService: SpinnerService
  ) { }

  ngOnInit(): void {
    const user:any = JSON.parse(localStorage.getItem('user'));
    if(user && user != "null" && new Date(user.user.expire) >= new Date()){
      this._router.navigate(["/"]);
    }
    this.loginForm = this._fb.group({
      userName: this._fb.control(null, [Validators.required]),
      passWord: this._fb.control(null, [Validators.required]),
    });
  }

  loginClick() {
    this.isSubmit = true;
    if (this.loginForm.invalid) {
      return;
    }
    this._spinnerService.start();
    this._userService.login(this.loginForm.value).subscribe(response => {
      if(response.success){
        this.store.dispatch(userActions.login({user:{...response.responseData }}));
        this._spinnerService.stop();
        this._router.navigate(["/"]);
      }
      else{
        this._spinnerService.stop();
        this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: response.message });
      }
    },error => {
      this._spinnerService.stop();
      this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: "Không thể connect đến server, vui lòng liên hệ quản trị viên để được hỗ trợ" });
    })
  }
}
