import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BiddingPackageComponent } from './components/bidding-package/bidding-package.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { DocumentSettingComponent } from './components/document-setting/document-setting.component';
import { DocumentComponent } from './components/document/document.component';
import { NotfoundComponent } from './components/notfound/notfound.component';
import { ProjectFlowComponent } from './components/project-flow/project-flow.component';
import { ProjectComponent } from './components/project/project.component';
import { DecentralizationComponent } from './components/users/decentralization/decentralization.component';
import { LoginComponent } from './components/users/login/login.component';
import { UserComponent } from './components/users/user/user.component';
import { AuthGuard } from './guard/auth-guard';


const routes: Routes = [
  { path: '', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'users', component: UserComponent, canActivate: [AuthGuard] },
  { path: 'project', component: ProjectComponent, canActivate: [AuthGuard] },
  { path: 'bidding-package', component: BiddingPackageComponent, canActivate: [AuthGuard] },
  { path: 'document', component: DocumentComponent, canActivate: [AuthGuard] },
  { path: 'project-flow/:projectId', component: ProjectFlowComponent, canActivate: [AuthGuard] },
  { path: 'document-setting/:projectId', component: DocumentSettingComponent, canActivate: [AuthGuard] },
  { path: 'decentralization/:userId', component: DecentralizationComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: '404', component: NotfoundComponent },
  { path: '**', redirectTo: '/', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
