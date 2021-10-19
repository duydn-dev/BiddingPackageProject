import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseService } from '../base/base-service.service';

@Injectable({
  providedIn: 'root'
})
export class BiddingService {

  constructor(
    private _baseService: BaseService
    ) {
  }
  getFilter(filter:any){
    return this._baseService.getWithQuery("api/biddingpackage","filter",JSON.stringify(filter))
  }
  getById(biddingPackageId:any){
    return this._baseService.get("api/biddingpackage/with-document", biddingPackageId);
  }
  getDropdown(){
    return this._baseService.get("api/biddingpackage/dropdown");
  }
  getPackageProjectId(projectId:any){
    return this._baseService.get("api/biddingpackage/dropdown-by-project", projectId)
  }
  create(biddingPackage:any){
    return this._baseService.post("api/biddingpackage/create", biddingPackage);
  }
  update(packageId, biddingPackage:any){
    return this._baseService.put("api/biddingpackage/update",packageId, biddingPackage);
  }
  delete(packageId){
    return this._baseService.delete("api/biddingpackage/delete",packageId);
  }
}
