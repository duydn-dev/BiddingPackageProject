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
import {MessageService, ConfirmationService, TreeNode} from 'primeng/api';
import { PrimeNgModule } from './modules/primeng.module';

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
import { BiddingPackageComponent } from './components/bidding-package/bidding-package.component';
import { DocumentComponent } from './components/document/document.component';
import { ProjectFlowComponent } from './components/project-flow/project-flow.component';



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
    BiddingPackageComponent,
    DocumentComponent,
    ProjectFlowComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    PrimeNgModule,
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
