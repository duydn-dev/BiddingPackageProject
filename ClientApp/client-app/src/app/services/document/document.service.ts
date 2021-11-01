import { Injectable } from '@angular/core';
import { BaseService } from '../base/base-service.service';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {

  constructor(
    private _baseService: BaseService
    ) {
  }
  getFilter(filter:any){
    return this._baseService.getWithQuery("api/document","filter",JSON.stringify(filter))
  }
  getDropDown(){
    return this._baseService.get("api/document/dropdown");
  }
  getDropDownByPackage(request:any){
    return this._baseService.post("api/document/dropdown-by-packageid", request);
  }
  getById(documentId:any){
    return this._baseService.get("api/document", documentId);
  }
  create(document:any){
    return this._baseService.post("api/document/create", document);
  }
  update(documentId, document:any){
    return this._baseService.put("api/document/update",documentId, document);
  }
  delete(documentId){
    return this._baseService.delete("api/document/delete", documentId);
  }

  getSettingDocument(documentId){
    return this._baseService.get("api/document/get-setting-document", documentId);
  }
}
