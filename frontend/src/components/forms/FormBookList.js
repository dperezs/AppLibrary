import { useEffect, useState } from "react";
import { endpoints } from '../../api/endpoints';
import { useNavigate  } from 'react-router-dom';
import {
    Card,
    Row,
    Col,
    CardTitle,
    CardBody,   
    Table,
    Button,
    Modal,
    ModalHeader, ModalBody, ModalFooter,
    Alert
  } from "reactstrap";
  
  const FormBookList = () => {

    const [modal, setModal] = useState(false);
    const [visible, setVisible] = useState(false);
    const toggle = () => setModal(!modal);

    const [listaBooks, setListaBooks] = useState([]);
    const [mensajeError, setMensajeError] = useState("");
    const history = useNavigate ();
    const [formCatalogo,setFormCatalogo] = useState({});
    const [modalEliminar,setmodalEliminar] = useState(false);
    const [estadoInicial,setEstadoInicial] = useState({});

    useEffect(()=>{
        Promise.all([
            endpoints.FormBook.GetAll()
        ])
            .then(res => {
                if (res.some(r => r.message)) {
                    setMensajeError('Ocurrio un error al cargar los datos');
                    return;
                }
                const [ents] = res;
                const books = ents.data
                    .map((i, index) => ({
                        ...i,
                        Index: index + 1,
                    }));

                    setListaBooks(books);
            });
            setEstadoInicial({ inicial : false});
    },[estadoInicial.inicial])
    const handleEditar = (item) => {
      history(
        '/formBookEdit',
        {state: {book: item}}
      );
    }
    const handleNew = () => {
      const tempForm = {
        id:0,
        name:'',
        noCopies:0,
        description:''
      }

      history(
        '/formBookEdit',
        {state: {book: tempForm}}
      );
    }
    const handleDelete = (item) => {
      setModal(!modal);
      const tempForm = {
          ...item
      };
      setFormCatalogo(tempForm);
    }
    const Delete = () => {
      setModal(!modal);
      endpoints.FormBook.Delete(formCatalogo.id)
      .then(response => {     
    
          if (response.message)                     
              alert('Ocurrio un error al cargar los datos');
          else                                      
          {          
            setVisible(true);
            setEstadoInicial({ inicial : true});
          }
              
      });
    }
    const onDismiss = () => {
      setVisible(false);
    };
    return (
      <>
      <Alert
        color="primary"
        isOpen={visible}
        toggle={onDismiss.bind(null)}
        fade={true}
        >
        ¡El registro se elimino correctamente!
    </Alert>
      <Row>
        <Col>
          {/* --------------------------------------------------------------------------------*/}
          {/* Card-1*/}
          {/* --------------------------------------------------------------------------------*/}
          <Card>
            <CardTitle tag="h6" className="border-bottom p-3 mb-0">
              <i className="bi bi-bell me-2"> </i>
              Form Example
            </CardTitle>
            <CardBody>
            <Col lg="12">
                    <Card>
                    <CardTitle tag="h6" className="border-bottom p-3 mb-0">
                        <i className="bi bi-card-text me-2"> </i>
                        Table with Striped    
                        <Row className="mt-3">
                        <Col lg="12">
                          <div className="bg-light p-2 border"><Button className="btn" color="primary" onClick={()=>handleNew()}> Agregar </Button></div>
                        </Col>  
                        </Row>                    
                        
                    </CardTitle>
                    <CardBody className="">
                        <Table bordered striped>
                        <thead>
                            <tr>
                            <th>Acciones</th>    
                            <th>#</th>
                            <th>Nombre</th>
                            <th>No. Copias</th>
                            <th>Descripción</th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                listaBooks.map((item, index) => (
                                    <tr key={index}>
                                    <td className="button-group">	
                                    <Button className="btn pr-3" color="primary"   onClick={() => handleEditar(item)}> Editar </Button>
                                    <Button className="btn" color="danger"  onClick={() => handleDelete(item)}> Eliminar </Button>                                    
                                    </td>
                                    <td>{item.Index}</td>
                                    <td>{item.name}</td>
                                    <td>{item.noCopies}</td>
                                    <td>{item.description}</td>                                    
                                </tr>
                            ))
                            }	 
                        </tbody>
                        </Table>
                    </CardBody>
                    </Card>
                </Col>
            </CardBody>
          </Card>
        </Col>
      </Row>
      <Modal isOpen={modal} toggle={toggle}>
        <ModalHeader toggle={toggle}>Modal title</ModalHeader>
        <ModalBody>
        Desea eliminar el registro?
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={()=>Delete()}>
            Do Something
          </Button>
          <Button color="secondary" onClick={toggle}>
            Cancel
          </Button>
        </ModalFooter>
      </Modal>
      
      </>
    );
  };
  
  export default FormBookList;
  