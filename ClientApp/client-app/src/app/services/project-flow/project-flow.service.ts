import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseService } from '../base/base-service.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectFlowService {

  constructor(
    private _baseService: BaseService
    ) {
  }

  createFlow(file:any, metadata:any) : Observable<any>{
    const formData = new FormData();
    if(file){
      formData.append('file', file);
    }
    formData.append('projectFlow', JSON.stringify(metadata));
    return this._baseService.uploadFile('api/projectflow/create', formData);
  }
  updateFlow(file:any, metadata:any) : Observable<any>{
    const formData = new FormData();
    if(file){
      formData.append('file', file);
    }
    formData.append('projectFlow', JSON.stringify(metadata));
    return this._baseService.uploadFile('api/projectflow/update', formData);
  }
  projectCurrentState(projectId: any){
    return this._baseService.get('api/projectflow/current-state', projectId);
  }
  currentPackage(projectId: any){
    return this._baseService.get('api/projectflow/current-package', projectId);
  }
  getFlows(request:any){
    return this._baseService.getWithQuery('api/projectflow', "filter", JSON.stringify(request));
  }
  getFlowById(flowId:any){
    return this._baseService.get('api/projectflow/get-by-id', flowId);
  }
  delete(flowId:any){
    return this._baseService.delete('api/projectflow/delete', flowId);
  }
}
