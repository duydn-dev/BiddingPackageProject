// đây là action
import { createAction, props } from '@ngrx/store';
import { User } from '../entities/user.entity';
export const USER_LOGIN = '[Login Page] Login';
export const USER_LOGOUT = '[Logout Page] Logout';
export const NOT_FOUND_PAGE = '[Not Found Page] Not Found';
export const OUT_NOT_FOUND_PAGE = '[Out Not Found Page] Not Found';

export const login = createAction(
  USER_LOGIN,
  props<{user: User}>()
);
export const logout = createAction(
  USER_LOGOUT
);
export const notFound = createAction(
  NOT_FOUND_PAGE
);
export const outNotFound = createAction(
  OUT_NOT_FOUND_PAGE
);