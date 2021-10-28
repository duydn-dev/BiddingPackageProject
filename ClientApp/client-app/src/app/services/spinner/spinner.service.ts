import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { State } from 'src/app/ngrx';
import * as spinnerActions from '../../ngrx/actions/spinner.action'

@Injectable({
  providedIn: 'root'
})
export class SpinnerService {
  constructor(
    private _store: Store<State>,
    ) {
  }
  start(){
    this._store.dispatch(spinnerActions.start({status: true}))
  }
  stop(){
    this._store.dispatch(spinnerActions.start({status: false}))
  }
}
