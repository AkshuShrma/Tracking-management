import { Component, OnInit } from '@angular/core';
import { Router,Event, NavigationStart } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { invokeBooksAPI, invokeDeleteBookAPI } from 'src/app/books/store/actions/books.actions';
import { selectBooks } from 'src/app/books/store/selectors/books.selector';
import { selectAppState } from 'src/app/shared/store/app.selector';
import { Appstate } from 'src/app/shared/store/appstate';

declare var window: any;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(
    private router:Router,
    private store: Store,
     private appStore: Store<Appstate>) {}

  books$ = this.store.pipe(select(selectBooks));

  deleteModal: any;
  idToDelete: number = 0;
 
  ngOnInit(): void {
    this.deleteModal = new window.bootstrap.Modal(
      document.getElementById('deleteModal')
    ); 
    this.store.dispatch(invokeBooksAPI());
  }

  openDeleteModal(id: number) {
    this.idToDelete = id;
    this.deleteModal.show();
  }

  delete() {
    this.store.dispatch(
      invokeDeleteBookAPI({
        bookId: this.idToDelete,
      })
    );
    let apiStatus$ = this.appStore.pipe(select(selectAppState));
    apiStatus$.subscribe((apState) => {
      if (apState.apiStatus == 'success') {
        this.deleteModal.hide();
        this.router.navigate(['home']);
      }
    });
  }

}
