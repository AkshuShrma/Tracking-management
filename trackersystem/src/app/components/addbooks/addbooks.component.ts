import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { Observable } from 'rxjs';
import { invokeSaveNewBookAPI } from 'src/app/books/store/actions/books.actions';
import { setAPIStatus } from 'src/app/shared/store/app.action';
import { selectAppState } from 'src/app/shared/store/app.selector';
import { Appstate } from 'src/app/shared/store/appstate';

@Component({
  selector: 'app-addbooks',
  templateUrl: './addbooks.component.html',
  styleUrls: ['./addbooks.component.scss']
})
export class AddbooksComponent implements OnInit {
  
  bookForm: any = {
    bookId: 0,
    author: '',
    name: '',
    cost: 0,
  };

  constructor(
    private store: Store,
    private appStore: Store<Appstate>,
    private router: Router
  ) {}

  ngOnInit(): void {
    
  }

  save() {
    this.store.dispatch(invokeSaveNewBookAPI({ newBook: this.bookForm }));
    let apiStatus$ = this.appStore.pipe(select(selectAppState));
    apiStatus$.subscribe((apState) => {
      if (apState.apiStatus == 'success') {
         this.router.navigate(['home']);
      }
    });
  }

}
