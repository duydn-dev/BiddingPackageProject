// đây là action
import { createAction, props } from '@ngrx/store';
export const SPINNER_START = '[SPINNER_START] Start';

export const start = createAction(
    SPINNER_START,
    props<{status: boolean}>()
);