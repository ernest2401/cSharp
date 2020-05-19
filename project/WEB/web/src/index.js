import React from 'react';
import ReactDOM from 'react-dom';
import { Switch, Route, BrowserRouter as Router } from 'react-router-dom';
import 'antd/dist/antd.css';
import './index.css';
import HomePage from './pages/home';
import { Provider } from 'react-redux';
import { store } from './redux/store';
import { isAuthorize, logOut } from './pages/home/reducer';
import LayoutPage from './pages/layout';
import ProfilePage from './pages/profile-page';
import AvailableBooksPage from './pages/available-books';
import BookDetailsPage from './pages/available-books/book-details';
import PageNotFound from './pages/page-not-found';
import PrivateRoute from './private-route';
import UsersListPage from './pages/users-list';
import ChatPage from './pages/chat';
import UsersMoreDetailPage from './pages/users-list/user-detail';

!isAuthorize() && isAuthorize().then(data => data == false && store.dispatch(logOut()));

ReactDOM.render(
  <Provider store={store} >
    <Router>
      <Switch>
        <Route exact path='/' component={HomePage} />
        <PrivateRoute exact path='/chat' component={ChatPage} />
        <PrivateRoute exact path='/profile' component={ProfilePage} />
        <PrivateRoute exact path='/books' component={AvailableBooksPage} />
        <PrivateRoute path="/book/:id" component={BookDetailsPage} />
        <PrivateRoute exact path='/users' component={UsersListPage} onlyAdmin />
        <PrivateRoute exact path='/user/:id' component={UsersMoreDetailPage} onlyAdmin />
        <Route exact path='*' component={PageNotFound} />
      </Switch>
    </Router>
  </Provider >,
  document.getElementById('root')
);