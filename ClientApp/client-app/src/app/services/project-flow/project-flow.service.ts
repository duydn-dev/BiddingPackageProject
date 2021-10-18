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
  getFilter(filter:any){
    return this._baseService.getWithQuery("api/biddingpackage","filter",JSON.stringify(filter))
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
