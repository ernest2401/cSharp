//React
import React from 'react';
import { connect } from 'react-redux';

//Antd
import { Spin, Input, Select, Form, Button, Row, Col, InputNumber, Modal } from 'antd';

//Other
import LayoutPage from '../layout';
import './index.scss';
import { getBooksDetails, setBookDetailsValueAction, updateBooksDetails, createBook, addAvailableBook } from './reducer';
import { BookGenre, BookCondition } from '../../resources/constants';

//Constants
const { Option } = Select;
const { TextArea } = Input;

class BookDetailsPageComponent extends React.Component {
    state = { count: 1, maxTermDays: 1 }

    componentDidMount() {
        this.props.match.params.id != 'create' ? this.props.getBooksDetails(this.props.match.params.id) : this.props.clearBookData({});
    }

    componentWillUpdate(prevProps) {
        if (!this.props.bookDetails && prevProps.bookDetails) {
            this.setState({ imageUrl: prevProps.bookDetails.imageUrl });
        }
    }

    componentWillUnmount() {
        this.props.clearBookData(null);
    }

    save = values => {
        this.props.match.params.id != 'create' ? this.props.updateBooksDetails({ ...values, id: this.props.bookDetails.id })
            : this.props.createBook(values);
    }

    render() {
        const { bookDetails, userData } = this.props;
        const genres = BookGenre.map(item => <Option key={item.key} value={item.key}>{item.text}</Option>);
        const conditions = BookCondition.map(item => <Option key={item.key} value={item.key}>{item.text}</Option>);
        const canEdit = userData ? userData.roles.includes('Admin') : false;

        return (
            <LayoutPage>
                <Modal
                    title="Add Available Book"
                    visible={this.state.visiblePanelAddAvailableBook}
                    footer={null}
                    onCancel={e => this.setState({ visiblePanelAddAvailableBook: false })}>
                    Count
                        <InputNumber className="full-width" style={{ marginBottom: '15px' }} min={1} value={this.state.count} onChange={e => this.setState({ count: e })} />

                        Max Term Days
                        <InputNumber className="full-width" min={1} value={this.state.maxTermDays} onChange={e => this.setState({ maxTermDays: e })} />

                    <Button style={{ marginTop: '25px' }} className="full-width" onClick={e => {
                        this.props.addAvailableBook({ count: this.state.count, maxTermDays: this.state.maxTermDays, bookId: this.props.bookDetails.id });
                        this.setState({ visiblePanelAddAvailableBook: false });
                    }}>Save</Button>
                </Modal>
                <Spin spinning={this.props.loading}>
                    {
                        bookDetails &&
                        <Form name="book-details-form" className="book-details-page" onFinish={this.save} initialValues={bookDetails}>
                            <Form.Item name="name" rules={[
                                {
                                    required: true,
                                    message: 'Please input Book Name',
                                },
                            ]}>
                                <Input readOnly={!canEdit} className="book-name" placeholder="Book Name" />
                            </Form.Item>

                            <Row style={{ padding: '30px' }}>
                                <Col xs={24} sm={24} md={12} lg={12} xl={12} className="left-panel">
                                    <Form.Item name="author" rules={[
                                        {
                                            required: true,
                                            message: 'Please input Author',
                                        },
                                    ]}>
                                        <Input readOnly={!canEdit} placeholder="Author" />
                                    </Form.Item>
                                    <Form.Item name="description" rules={[
                                        {
                                            required: true,
                                            message: 'Please input Description',
                                        },
                                    ]}>
                                        <TextArea readOnly={!canEdit} rows={4} placeholder="Description" />
                                    </Form.Item>
                                    <Form.Item name="genre" className="to-left" rules={[{ required: true, message: 'Please choose genre' }]}>
                                        <Select placeholder="Genre" readOnly={!canEdit}>
                                            {genres}
                                        </Select>
                                    </Form.Item>
                                    <Form.Item className="to-left" name="condition" rules={[{ required: true, message: 'Please choose condition' }]}>
                                        <Select placeholder="Condition" readOnly={!canEdit}>
                                            {conditions}
                                        </Select>
                                    </Form.Item>
                                    <Form.Item name="pages" rules={[{ required: true, message: 'Please input pages number' }]}>
                                        <InputNumber
                                            readOnly={!canEdit}
                                            className="full-width"
                                            min={0}
                                            formatter={value => `${value} pages`}
                                            parser={value => value.replace(' pages', '')}
                                        />
                                    </Form.Item>
                                </Col>
                                <Col xs={24} sm={24} md={12} lg={12} xl={12} className="left-panel">
                                    {
                                        this.state.imageUrl ? <img style={{ objectFit: 'cover' }} src={this.state.imageUrl} height="350px" width="350px" /> : <div />
                                    }
                                    {
                                        canEdit &&
                                        <Form.Item name="imageUrl">
                                            <Input style={{ marginTop: '15px', width: '50%' }} onChange={e => this.setState({ imageUrl: e.target.value })} readOnly={!canEdit} placeholder="Image Url" />
                                        </Form.Item>
                                    }
                                </Col>
                            </Row>
                            {canEdit && this.props.match.params.id != 'create' && <Button onClick={e => this.setState({ visiblePanelAddAvailableBook: true })} style={{ marginRight: '35px' }}>Add book to available</Button>}
                            {canEdit && <Button className="save-changes-btn" htmlType="submit">{this.props.match.params.id == "create" ? "Create Book" : "Save changes"}</Button>}
                        </Form>
                    }
                </Spin>
            </LayoutPage>
        )
    }
}

const mapState = ({ books, home }) => {
    return {
        bookDetails: books.bookDetails,
        userData: home.userData,
        loading: books.loading
    }
};

const mapDispatch = (dispatch) => {
    return {
        getBooksDetails(id) {
            dispatch(getBooksDetails(id));
        },
        clearBookData(value) {
            dispatch(setBookDetailsValueAction(value));
        },
        updateBooksDetails(model) {
            dispatch(updateBooksDetails(model));
        },
        createBook(model) {
            dispatch(createBook(model));
        },
        addAvailableBook(model) {
            dispatch(addAvailableBook(model));
        }
    };
};

const BookDetailsPage = connect(mapState, mapDispatch)(BookDetailsPageComponent);
export default BookDetailsPage;