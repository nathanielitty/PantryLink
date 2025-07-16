import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PantryDetail } from './pantry-detail';

describe('PantryDetail', () => {
  let component: PantryDetail;
  let fixture: ComponentFixture<PantryDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PantryDetail]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PantryDetail);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
