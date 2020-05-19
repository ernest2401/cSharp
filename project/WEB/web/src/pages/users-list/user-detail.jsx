//React
import React from 'react';
import { connect } from 'react-redux';

//Antd
import { Table } from 'antd';

//Other
import moment from 'moment';
import './index.scss';
import { Link } from 'react-router-dom';
import ProfilePage from '../profile-page';
import { getReservedBooks } from '../available-books/reducer';
import { BookGenre } from '../../resources/constants';

//Constants

class UsersMoreDetailPageComponent extends React.Component {
    state = {}

    componentDidMount() {
        this.props.getReservedBooks({ userId: this.props.match.params.id });
    }

    getColumnsForReservedBook = () => {
        return [
            {
                title: 'Image',
                dataIndex: ['availableBook', 'book', 'imageUrl'],
                render: text => text ? <img style={{ objectFit: 'cover' }} src={text} width="100px" height="100px" /> : <div />
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

            }
        ];

    }

    render() {
        return (
            <div>
                <ProfilePage userId={this.props.match.params.id} />
                {
                    this.props.reservedBooks &&
                    <div style={{ margin: '25px', marginTop: '50px', textAlign: 'center' }}>
                        <h2>Reserved Books</h2>
                        <Table columns={this.getColumnsForReservedBook()} dataSource={this.props.reservedBooks} />
                    </div>
                }
            </div>
        )
    }
}

const mapState = ({ home, books }) => {
    return {
        loading: home.loading,
        usersList: home.usersList,
        reservedBooks: books.reservedBooks
    }
};

const mapDispatch = (dispatch) => {
    return {
        getReservedBooks(model) {
            dispatch(getReservedBooks(model));
        }
    };
};

const UsersMoreDetailPage = connect(mapState, mapDispatch)(UsersMoreDetailPageComponent);
export default UsersMoreDetailPage;