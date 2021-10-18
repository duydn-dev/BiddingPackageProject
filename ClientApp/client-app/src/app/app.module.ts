import { BrowserModule } from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule }   from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/users/login/login.component';
import { StoreModule } from '@ngrx/store';
import {metaReducers, reducers} from '../app/ngrx/index';

// primNG module
import {ToastModule} from 'primeng/toast';
import {TableModule} from 'primeng/table';
import {MessageService, ConfirmationService, TreeNode} from 'primeng/api';
import {PaginatorModule} from 'primeng/paginator';
import {DialogModule} from 'primeng/dialog';
import {ConfirmDialogModule} from 'primeng/confirmdialog';
import {InputTextModule} from 'primeng/inputtext';
import {DropdownModule} from 'primeng/dropdown';
import {FileUploadModule} from 'primeng/fileupload';
import {TreeModule} from 'primeng/tree';
import {CalendarModule} from 'primeng/calendar';
import {InputTextareaModule} from 'primeng/inputtextarea';

// component
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NotfoundComponent } from './components/notfound/notfound.component';
import { AuthGuard } from './guard/auth-guard';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { MenuLeftComponent } from './components/menu-left/menu-left.component';
import { MenuTopComponent } from './components/menu-top/menu-top.component';
import { UserComponent } from './components/users/user/user.component';
import { ProjectComponent } from './components/project/project.component';
import { DecentralizationComponent } from './components/users/decentralization/decentralization.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardComponent,
    NotfoundComponent,
    MenuLeftComponent,
    MenuTopComponent,
    UserComponent,
    ProjectComponent,
    DecentralizationComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    ToastModule,
    TableModule,
    PaginatorModule,
    DialogModule,
    ConfirmDialogModule,
    InputTextModule,
    DropdownModule,
    FileUploadModule,
    TreeModule,
    CalendarModule,
    InputTextareaModule,
    StoreModule.forRoot(reducers, {metaReducers})
  ],
  providers: [
    MessageService,
    ConfirmationService,
    AuthGuard,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
