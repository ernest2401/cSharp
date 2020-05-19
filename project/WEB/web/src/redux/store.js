// React
import { createStore, applyMiddleware } from 'redux';
import { rootReducer } from './reducers';
import thunk from 'redux-thunk';

//Create store and connect the devtools extension
export const store = createStore(rootReducer, applyMiddleware(thunk));
