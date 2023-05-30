import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BooksRoutingModule } from './books-routing.module';
import { StoreModule } from '@ngrx/store';
import { bookReducer } from './store/reducers/books.reducer';
import { EffectsModule } from '@ngrx/effects';
import { BooksEffect } from './store/effects/books.effect';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BooksRoutingModule,
    EffectsModule.forFeature(BooksEffect),
    StoreModule.forFeature('mybooks', bookReducer),
  ]
})
export class BooksModule { }
