//React
import React from 'react';
import { connect } from 'react-redux';

//Antd
import { Row, Col, Table, Spin, Input, Select, Button, Modal, Tabs } from 'antd';

//Other
import LayoutPage from '../layout';
import moment from 'moment';
import './index.scss';
import { getBooksList, deleteBook, getAvailableBooksList, deleteAvailableBook, getReservedBooks, returnBook, reserveBook } from './reducer';
import { Link } from 'react-router-dom';
import { BookGenre, BookCondition } from '../../resources/constants';

//Constants
const { Option } = Select;
const { confirm } = Modal;
const { TabPane } = Tabs;

class AvailableBooksPageComponent extends React.Component {
    state = {}

    componentDidMount() {
        this.props.userRoles.includes('Admin') && this.props.getBooksList({});
        this.props.getAvailableBooksList({});
        this.props.getReservedBooks({ userId: this.props.userId })
    }

    deleteBook = id => {
        confirm({
            title: 'Are you really want to delete book?',
            onOk: () => this.onDelete(id)
        });
    }

    deleteAvailableBook = id => {
        confirm({
            title: 'Are you really want to delete available book?',
            onOk: () => this.onDeleteAvailable(id)
        });
    }

    returnBook = id => {
        confirm({
            title: 'Are you really want to return book?',
            onOk: () => this.onReturn(id)
        });
    }

    onDeleteAvailable = id => {
        this.props.deleteAvailableBook(id, this.state);
    }

    onDelete = id => {
        this.props.deleteBook(id, this.state);
    }

    onReturn = id => {
        this.props.returnBook(id, { ...this.state, userId: this.props.userId });
    }

    getColumnsForAvailableBook = () => {
        return [
            {
                title: 'Image',
                dataIndex: ['book', 'imageUrl'],
                render: text => <img style={{ objectFit: 'cover' }} src={text ? text : 'https://thumbs.dreamstime.com/b/no-image-available-icon-flat-vector-no-image-available-icon-flat-vector-illustration-132484366.jpg'} width="100px" height="100px" />
            },
            {
                title: 'Name',
                dataIndex: ['book', 'name'],
                key: 'name',
                render: (text, item) => <Link to={`/book/${item.book.id}`}>{text}</Link>,
            },
            {
                title: 'Author',
                dataIndex: ['book', 'author'],
                key: 'author',
            },
            {
                title: 'Genre',
                dataIndex: ['book', 'genre'],
                key: 'genre',
                render: text => BookGenre.find(item => item.key == text).text
            },
            {
                title: 'Description',
                dataIndex: ['book', 'description']
            },
            {
                title: 'Count',
                dataIndex: 'count'
            },
            {
                title: 'Reserved',
                dataIndex: 'reservedBooksCount'
            },
            {
                title: 'Max Days For Reserve',
                dataIndex: 'maxTermDays'
            },
            {
                render: item => <Button
                    onClick={e => this.props.reserveBook({ availableBookId: item.id, userId: this.props.userId }, { ...this.state, userId: this.props.userId })}
                    type="primary" ghost>Reserve Book</Button>
            },
            {
                dataIndex: ['book', 'isActive'],
                render: (text, item) => text && this.props.userRoles.includes('Admin') ? <Button onClick={e => this.deleteAvailableBook(item.id)} type="danger" ghost>Delete</Button> : <div />
            }
        ];
    }

    getColumnsForReservedBook = () => {
        return [
            {
                title: 'Image',
                dataIndex: ['availableBook', 'book', 'imageUrl'],
                render: text => <img style={{ objectFit: 'cover' }} src={text ? text : 'https://thumbs.dreamstime.com/b/no-image-available-icon-flat-vector-no-image-available-icon-flat-vector-illustration-132484366.jpg'} width="100px" height="100px" />
            },
            {
                title: 'Name',
                dataIndex: ['availableBook', 'book', 'name'],
                key: 'name',
                render: (text, item) => <Link to={`/book/${item.availableBook.book.id}`}>{text}</Link>,
            },
            {
                title: 'Author',
                dataIndex: ['availableBook', 'book', 'author'],
                key: 'author',
            },
            {
                title: 'Genre',
                dataIndex: ['availableBook', 'book', 'genre'],
                key: 'genre',
                render: text => BookGenre.find(item => item.key == text).text
            },
            {
                title: 'Description',
                dataIndex: ['availableBook', 'book', 'description']
            },
            {
                title: 'Reserved At',
                dataIndex: 'reservedAt',
                render: value => <div>{moment(value).format("DD/MM/YYYY")} {moment(value).format('LT')} </div>

            },
            {
                // dataIndex: ['book', 'isActive'],
                render: (item) => <Button type="danger" onClick={e => this.returnBook(item.id)} ghost>Return Book</Button>
            }
        ];
    }

    getColumns = () => {
        return [
            {
                title: 'Image',
                dataIndex: 'imageUrl',
                render: text => <img style={{ objectFit: 'cover' }} src={text ? text : 'https://thumbs.dreamstime.com/b/no-image-available-icon-flat-vector-no-image-available-icon-flat-vector-illustration-132484366.jpg'} width="100px" height="100px" />
            },
            {
                title: 'Name',
                dataIndex: 'name',
                key: 'name',
                render: (text, item) => <Link to={`/book/${item.id}`}>{text}</Link>,
            },
            {
                title: 'Author',
                dataIndex: 'author',
                key: 'author',
            },
            {
                title: 'Genre',
                dataIndex: 'genre',
                key: 'genre',
                render: text => BookGenre.find(item => item.key == text).text
            },
            {
                title: 'Description',
                dataIndex: 'description'
            },
            {
                dataIndex: 'isActive',
                render: (text, item) => text && this.props.userRoles.includes('Admin') ? <Button onClick={e => this.deleteBook(item.id)} type="danger" ghost>Delete</Button> : <div />
            }
        ];
    }

    onFilter = () => {
        this.props.userRoles.includes('Admin') && this.props.getBooksList(this.state);
        this.props.getAvailableBooksList(this.state);
    }

    render() {
        const genres = BookGenre.map(item => <Option key={item.key} value={item.key}>{item.text}</Option>);
        const conditions = BookCondition.map(item => <Option key={item.key} value={item.key}>{item.text}</Option>);

        return (
            <LayoutPage>
                <Spin spinning={this.props.loading} >
                    <div className="available-books-list-page">
                        <Row>
                            <Col xs={24} sm={24} md={17} lg={17} xl={17}>
                                <h1>Books</h1>
                            </Col>
                            <Col xs={24} sm={24} md={7} lg={7} xl={7} className="right-panel-header">
                                {this.props.userRoles.includes('Admin') && <Button style={{ marginBottom: '25px', float: 'left', color: 'white', background: '#686058' }}><Link to={'/book/create'}>Add book to library</Link></Button>}
                            </Col>
                        </Row>
                        <Row>
                            <Col xs={24} sm={24} md={17} lg={17} xl={17} className="left-panel">
                                <Tabs defaultActiveKey="1" type="card">
                                    <TabPane tab="Available Books" key="1">
                                        {
                                            this.props.availableBooks &&
                                            <Table columns={this.getColumnsForAvailableBook()} dataSource={this.props.availableBooks} />
                                        }
                                    </TabPane>
                                    {
                                        this.props.books && this.props.userRoles.includes('Admin') &&
                                        <TabPane tab="All Books" key="2">
                                            <Table columns={this.getColumns()} dataSource={this.props.books} />
                                        </TabPane>
                                    }
                                    <TabPane tab="Reserved Books" key="3">
                                        {
                                            this.props.reservedBooks &&
                                            <Table columns={this.getColumnsForReservedBook()} dataSource={this.props.reservedBooks} />
                                        }
                                    </TabPane>
                                </Tabs>
                            </Col>
                            <Col xs={24} sm={24} md={7} lg={7} xl={7} className="right-panel">
                                <h2>Filter</h2>
                                <Input className="input-margin" placeholder="Search" onChange={e => this.setState({ searchString: e.target.value })} />
                                <Select allowClear className="full-width input-margin" placeholder="Genre" onChange={e => this.setState({ genre: e })}>
                                    {genres}
                                </Select>
                                <Select allowClear className="full-width input-margin" placeholder="Condition" onChange={e => this.setState({ condition: e })}>
                                    {conditions}
                                </Select>
                                <Button className="full-width input-margin" onClick={this.onFilter}>Filter</Button>
                            </Col>
                        </Row>
                    </div>
                </Spin>
            </LayoutPage>
        )
    }
}

const mapState = ({ books, home }) => {
    return {
        books: books.booksList,
        loading: books.loading,
        userRoles: home.userData ? home.userData.roles || [] : [],
        userId: home.userData.id,
        availableBooks: books.availableBooks,
        reservedBooks: books.reservedBooks
    }
};

const mapDispatch = (dispatch) => {
    return {
        getBooksList(model) {
            dispatch(getBooksList(model));
        },
        deleteBook(id, model) {
            dispatch(deleteBook(id, model));
        },
        getAvailableBooksList(model) {
            dispatch(getAvailableBooksList(model));
        },
        deleteAvailableBook(id, model) {
            dispatch(deleteAvailableBook(id, model));
        },
        getReservedBooks(model) {
            dispatch(getReservedBooks(model));
        },
        returnBook(id, model) {
            dispatch(returnBook(id, model));
        },
        reserveBook(model, filter) {
            dispatch(reserveBook(model, filter));
        }
    };
};

const AvailableBooksPage = connect(mapState, mapDispatch)(AvailableBooksPageComponent);
export default AvailableBooksPage;