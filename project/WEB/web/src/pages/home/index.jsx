//React
import React from 'react';
import { connect } from 'react-redux';

//Antd
import { Input, Row, Col, Button, Form, Modal, Select, DatePicker, Spin } from 'antd';

//Other
import './index.scss';
import { registration, login } from './reducer';
import { Link } from 'react-router-dom';

//Constants
const { Option } = Select;

class HomePageComponent extends React.Component {
    state = {}

    login = values => {
        this.props.login(values);
    }

    registration = values => {
        this.props.registration(values);

        setTimeout(() => {
            this.setState({ visiblePanelRegistration: false })
        }, 0)
    }

    render() {
        return (
            <div className="main-page">
                <Spin spinning={this.props.loading}>
                    <Modal
                        title="Registration"
                        visible={this.state.visiblePanelRegistration}
                        footer={null}
                        onCancel={e => this.setState({ visiblePanelRegistration: false })}>
                        <Spin spinning={this.props.loading}>
                            <Form name="refistration-form" onFinish={this.registration} className="registration-modal">
                                <Form.Item name="firstName" rules={[
                                    {
                                        required: true,
                                        message: 'Please input your First Name',
                                    },
                                ]}>
                                    <Input id="firstName" placeholder="First Name" />
                                </Form.Item>
                                <Form.Item name="lastName" rules={[
                                    {
                                        required: true,
                                        message: 'Please input your Last Name',
                                    },
                                ]}>
                                    <Input id="secondName" placeholder="Last Name" />
                                </Form.Item>
                                <Form.Item name="email" rules={[
                                    {
                                        type: 'email',
                                        message: 'The input is not valid Email',
                                    },
                                    {
                                        required: true,
                                        message: 'Please input your Email',
                                    },
                                ]}>
                                    <Input id="email" placeholder="Email" />
                                </Form.Item>
                                <Form.Item name="phoneNumber" rules={[
                                    {
                                        required: true,
                                        message: 'Please input your Phone Number',
                                    },
                                ]}>
                                    <Input id="phoneNumber" placeholder="Phone Number" />
                                </Form.Item>
                                <Form.Item className="text-left" name="sex" rules={[{ required: true, message: 'Please choose your gender' }]}>
                                    <Select placeholder="Gender">
                                        <Option value={1}>Male</Option>
                                        <Option value={2}>Female</Option>
                                    </Select>
                                </Form.Item>
                                <Form.Item name="birthday" rules={[
                                    {
                                        required: true,
                                        message: 'Please input your Birthday Date',
                                    },
                                ]}>
                                    <DatePicker className="full-width" />
                                </Form.Item>
                                <Form.Item name="password" rules={[
                                    {
                                        required: true,
                                        message: 'Please input your password',
                                    },
                                ]}>
                                    <Input.Password id="password" className="password" placeholder="Password" />
                                </Form.Item>
                                <Button className="registration-btn" htmlType="submit">Sign Up</Button>
                            </Form>
                        </Spin>
                    </Modal>
                    <Row>
                        <Col xs={24} sm={24} md={this.props.userData ? 24 : 17} lg={this.props.userData ? 24 : 17} xl={this.props.userData ? 24 : 17} className="left-panel">
                            <h1 className="big-word">Welcome</h1>
                            <h1 className="big-word">to</h1>
                            <h1 className="big-word"><i>Smart Library</i></h1>
                            {
                                this.props.userData &&
                                <Button size="large" className="start-work-btn" ghost><Link to="/books">Start work</Link></Button>
                            }
                        </Col>
                        {!this.props.userData &&
                            <Col xs={24} sm={24} md={7} lg={7} xl={7} className="right-panel">
                                <Form onFinish={this.login} name="login-form">
                                    <h1 className="smart-library-text">Smart Library</h1>
                                    <Form.Item name="email" rules={[
                                        {
                                            type: 'email',
                                            message: 'The input is not valid Email',
                                        },
                                        {
                                            required: true,
                                            message: 'Please input your Email',
                                        },
                                    ]}>
                                        <Input id="email" placeholder="Email" />
                                    </Form.Item>
                                    <Form.Item name="password" rules={[
                                        {
                                            required: true,
                                            message: 'Please input your password',
                                        },
                                    ]}>
                                        <Input.Password id="password" className="password" placeholder="Password" />
                                    </Form.Item>
                                    <Button className="sign-in" htmlType="submit">Sign in</Button>
                                    <div className="register-message">
                                        If you are not already in the system, please
                                <Button type="link" onClick={e => this.setState({ visiblePanelRegistration: true })}>register</Button>
                                    </div>
                                </Form>
                            </Col>
                        }
                    </Row>
                </Spin>
            </div>
        )
    }
}

const mapState = ({ home }) => {
    return {
        loading: home.loading,
        userData: home.userData
    }
};

const mapDispatch = (dispatch) => {
    return {
        registration(model) {
            dispatch(registration(model));
        },
        login(model) {
            dispatch(login(model));
        }
    };
};

const HomePage = connect(mapState, mapDispatch)(HomePageComponent);
export default HomePage;