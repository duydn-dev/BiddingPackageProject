import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { State } from 'src/app/ngrx';
import * as userActions from '../../ngrx/actions/login.action';
declare var $:any;

@Component({
  selector: 'app-menu-top',
  templateUrl: './menu-top.component.html',
  styleUrls: ['./menu-top.component.css']
})
export class MenuTopComponent implements OnInit {

  currentUser:any = {};
  constructor(
    private _store : Store<State>,
    private _router: Router
  ) { }

  ngOnInit(): void {
    $('.avatar-dropdown').on('click', function(){
      $('.mdl-menu__container.is-upgraded').toggleClass("is-visible");
    });

    $('#btn-profile').on('click', function () {
      $(this).parents('.box-profile').toggleClass('open');
    });


    this._store.subscribe(n => {
      this.currentUser = n.user?.user;
    })
  }
  logout(){
    this._store.dispatch(userActions.logout());
    this._router.navigate(["/login"]);
  }
}
