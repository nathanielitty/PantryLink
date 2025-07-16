import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PantrySearch } from './pantry-search';

describe('PantrySearch', () => {
  let component: PantrySearch;
  let fixture: ComponentFixture<PantrySearch>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PantrySearch]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PantrySearch);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
