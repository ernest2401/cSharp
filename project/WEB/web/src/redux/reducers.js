// React
import { combineReducers } from "redux";

//Reducers
import { homeReducer } from "../pages/home/reducer";
import { booksReducer } from "../pages/available-books/reducer";
import { routerReducer } from "react-router-redux";

export const rootReducer = combineReducers({
  routing: routerReducer,
  home: homeReducer,
  books: booksReducer
});
