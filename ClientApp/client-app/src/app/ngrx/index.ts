import { ActionReducer, ActionReducerMap, MetaReducer } from '@ngrx/store';
import { localStorageSync } from 'ngrx-store-localstorage';
import * as fromUser from './reducers/user.reducer';

const environment:any = {
  production: false
}
export interface State {
    user: fromUser.State;
}
export const reducers: ActionReducerMap<State> = {
    user: fromUser.reducer,
};

const reducerKeys = ['user'];
export function localStorageSyncReducer(reducer: ActionReducer<any>): ActionReducer<any> {
  return localStorageSync({keys: reducerKeys})(reducer);
}
// console.log all actions
export function debug(reducer: ActionReducer<any>): ActionReducer<any> {
    return function(state, action) {
      // console.log('state', state);
      // console.log('action', action);
   
      return reducer(state, action);
    };
  }
   
export const metaReducers: MetaReducer<State>[] = !environment.production ? [debug, localStorageSyncReducer] : [localStorageSyncReducer];