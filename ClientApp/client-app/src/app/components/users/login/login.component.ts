import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Router } from '@angular/router';

import { State } from 'src/app/ngrx/reducers/user.reducer';
import * as userActions from '../../../ngrx/actions/login.action';
import { UserService } from '../../../services/user/user.service';
import { MessageService } from 'primeng/api';


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
    private _messageService: MessageService
  ) { }

  ngOnInit(): void {
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
    this._userService.login(this.loginForm.value).subscribe(response => {
      if(response.success){
        this.store.dispatch(userActions.login({user:{...response.responseData }}));
        this._router.navigate(["/"]);
      }
      else{
        this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: response.message });
      }
    })
  }
}
