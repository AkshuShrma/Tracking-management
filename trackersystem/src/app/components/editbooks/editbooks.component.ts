import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { switchMap } from 'rxjs';
import { invokeUpdateBookAPI } from 'src/app/books/store/actions/books.actions';
import { selectBookById } from 'src/app/books/store/selectors/books.selector';
import { setAPIStatus } from 'src/app/shared/store/app.action';
import { selectAppState } from 'src/app/shared/store/app.selector';
import { Appstate } from 'src/app/shared/store/appstate';

@Component({
  selector: 'app-editbooks',
  templateUrl: './editbooks.component.html',
  styleUrls: ['./editbooks.component.scss'],
})
export class EditbooksComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private store: Store,
    private appStore: Store<Appstate>
  ) {}

  bookForm: any = {
    id: 0,
    author: '',
    name: '',
    cost: 0,
  };

  ngOnInit(): void {
    // let id = this.route.snapshot.paramMap.get('id');
    // console.log(id)
    let fetchData$ = this.route.paramMap.pipe(
      switchMap((params) => {
        var bookId = Number(params.get('bookId'));
        return this.store.pipe(select(selectBookById(bookId)));
      })
    );
    fetchData$.subscribe((data) => {
      if (data) {
        this.bookForm = { ...data };
      } else {
        this.router.navigate(['/']);
      }
    });
  }

  ssssss() {
    this.store.dispatch(
      invokeUpdateBookAPI({ updateBook: { ...this.bookForm } })
    );
    let apiStatus$ = this.appStore.pipe(select(selectAppState));
    apiStatus$.subscribe((apState) => {
      if (apState.apiStatus == 'success') {
        this.appStore.dispatch(
          setAPIStatus({ apiStatus: { apiResponseMessage: '', apiStatus: '' } })
        );
        this.router.navigate(['home']);
      }
    });
  }
}
