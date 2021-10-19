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
}
