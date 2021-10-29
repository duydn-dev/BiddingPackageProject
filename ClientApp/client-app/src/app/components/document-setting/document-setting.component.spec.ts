import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentSettingComponent } from './document-setting.component';

describe('DocumentSettingComponent', () => {
  let component: DocumentSettingComponent;
  let fixture: ComponentFixture<DocumentSettingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DocumentSettingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
