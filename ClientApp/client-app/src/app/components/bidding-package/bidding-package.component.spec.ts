import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BiddingPackageComponent } from './bidding-package.component';

describe('BiddingPackageComponent', () => {
  let component: BiddingPackageComponent;
  let fixture: ComponentFixture<BiddingPackageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BiddingPackageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BiddingPackageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
