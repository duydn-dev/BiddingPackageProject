import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import * as userActions from '../../ngrx/actions/login.action';

@Component({
  selector: 'app-notfound',
  templateUrl: './notfound.component.html',
  styleUrls: ['./notfound.component.css']
})
export class NotfoundComponent implements OnInit, OnDestroy {

  constructor(
    private _store: Store
  ) { }
  ngOnDestroy(): void {
    this._store.dispatch(userActions.outNotFound());
  }

  ngOnInit(): void {
    this._store.dispatch(userActions.notFound());
  }
  
}
