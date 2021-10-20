import { Injectable } from '@angular/core';
import { BaseService } from '../base/base-service.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectFlowService {

  constructor(
    private _baseService: BaseService
    ) {
  }

  createFlow(projectFlow: any){
    return this._baseService.post('api/projectflow/create', projectFlow);
  }
  projectCurrentState(projectId: any){
    return this._baseService.get('api/projectflow/current-state', projectId);
  }
}
