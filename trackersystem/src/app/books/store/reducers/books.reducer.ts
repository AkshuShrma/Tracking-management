import { createReducer, on } from '@ngrx/store';
import {
  booksFetchAPISuccess,
  deleteBookAPISuccess,
  saveNewBookAPISucess,
  updateBookAPISucess,
} from '../actions/books.actions';

export const initialState: any = [];

export const bookReducer = createReducer(
  initialState,
  on(booksFetchAPISuccess, (state, { allBooks }) => {
    return allBooks;
  }),
  on(saveNewBookAPISucess, (state, { newBook }) => {
    let newState = [...state];
    newState.unshift(newBook);
    return newState;
  }),
  on(deleteBookAPISuccess, (state, { bookId }) => {
    let newState = [...state];
    let data = newState.filter((data) => {
      if (data.bookId != bookId) {
        return data;
      }
    });
    return data;
  }),
  // on(updateBookAPISucess, (state, { updateBook }) => {
  //   let newState = state.filter((data:any) => data.id == updateBook.id);
  //   newState.unshift(updateBook);
  //   return newState;
  // })
  on(updateBookAPISucess, (state, { updateBook }) => {
    let newState = [...state];
    debugger
    let data = newState.map((datas) => {
      if (datas.bookId == updateBook.bookId) {
        return updateBook;
      }
      return datas;
    });
    return data;
  })
);
