import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectFlowComponent } from './project-flow.component';

describe('ProjectFlowComponent', () => {
  let component: ProjectFlowComponent;
  let fixture: ComponentFixture<ProjectFlowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectFlowComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectFlowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
