import { TestBed } from '@angular/core/testing';

import { ProjectFlowService } from './project-flow.service';

describe('ProjectFlowService', () => {
  let service: ProjectFlowService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProjectFlowService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
