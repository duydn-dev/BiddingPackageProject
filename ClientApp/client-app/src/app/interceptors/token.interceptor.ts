import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Store } from "@ngrx/store";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";
import { map} from 'rxjs/operators';
import { State } from "../ngrx";
import * as userActions from '../ngrx/actions/login.action';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
    constructor(
        private _store : Store<State>,
        private _messageService: MessageService,
        private _router: Router
        ) {
    }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let user:any = JSON.parse(localStorage.getItem("user"));
        if(user){
            const modifiedReq = req.clone({ 
                headers: req.headers.set('Authorization', `Bearer ${user.user.token}`),
            });
            return next.handle(modifiedReq).pipe(map(event => {
                if (event instanceof HttpResponse) {
                    event = event.clone({ body: event.body })
                    if(event.status == 401){
                        this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: "thời gian hoạt động đã hết, hệ thống sẽ tự chuyển hướng về trang đăng nhập !" });
                        setTimeout(() => {
                            this._store.dispatch(userActions.logout());
                            this._router.navigate(["/login"]);
                        }, 3000);
                    }
                }         
                return event;
            }));
        }
        return next.handle(req);
    }
}