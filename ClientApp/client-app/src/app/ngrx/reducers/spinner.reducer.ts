import { Action, createReducer, on } from '@ngrx/store';
import * as spinerActions from '../actions/spinner.action';

export interface State {
    isLoading: boolean;
}
export const initialState: State = {
    isLoading : false
};

const spinnerReducer = createReducer(
    initialState,
    on(spinerActions.start, (state, prop) => ({
        isLoading: prop.status
    })),
);

export function reducer(state: State | undefined, action: Action): any {
    return spinnerReducer(state, action);
}