import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseService } from '../base/base-service.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor(
    private _baseService: BaseService
  ) { }

  getFilter(filter:any) :Observable<any>{
    return this._baseService.getWithQuery("api/project", "filter", JSON.stringify(filter));
  }
  getById(projectId:any){
    return this._baseService.get("api/project", projectId);
  }
  create(project:any){
    return this._baseService.post('api/project/create', project);
  }
  update(projectId:any, project:any){
    return this._baseService.put('api/project/update', projectId, project);
  }
  delete(projectId:any){
    return this._baseService.delete('api/project/delete', projectId);
  }
  exportExcel(projectId:any){
    return this._baseService.get('api/project/export-excel', projectId);
  }
  getStatistical(){
    return this._baseService.get('api/project/get-project-statistical');
  }
}
