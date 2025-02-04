import { useEffect, useRef, useState } from 'react';
import Matter from 'matter-js';
import { useDrag } from 'react-use-gesture';

const PhysicsWorld = ({ children }) => {
  const boxRef = useRef(null);
  const engine = useRef(Matter.Engine.create());
  const [balls, setBalls] = useState([]);

  useEffect(() => {
    const initialBalls = [
      { id: 1, x: 100, y: 100, image: '/carrot.png' },
      { id: 2, x: 200, y: 100, image: '/apple.png' }
    ];

    const matterBalls = initialBalls.map(ball => {
      const body = Matter.Bodies.circle(ball.x, ball.y, 30, {
        restitution: 0.6,
        frictionAir: 0.1,
        render: { visible: false }
      });
      return { ...ball, body };
    });

    const ground = Matter.Bodies.rectangle(400, 610, 810, 60, { isStatic: true });
    Matter.Composite.add(engine.current.world, [...matterBalls.map(b => b.body), ground]);
    
    const runner = Matter.Runner.create();
    Matter.Runner.run(runner, engine.current);

    setBalls(matterBalls);

    return () => Matter.Runner.stop(runner);
  }, []);

  return (
    <div ref={boxRef} style={{ 
      width: '800px', 
      height: '600px', 
      position: 'relative',
      overflow: 'hidden',
      touchAction: 'none'
    }}>
      {balls.map(ball => (
        <DraggableBall
          key={ball.id}
          body={ball.body}
          engine={engine.current}
          image={ball.image}
          containerRef={boxRef}
        />
      ))}
    </div>
  );
};

const DraggableBall = ({ body, engine, image, containerRef }) => {
  const [pos, setPos] = useState({ x: body.position.x, y: body.position.y });
  const [dragPos, setDragPos] = useState(null);
  const isDragging = useRef(false);
  const initialRect = useRef(null);

  useEffect(() => {
    const syncPosition = () => {
      if (!isDragging.current) {
        setPos({ x: body.position.x, y: body.position.y });
      }
      requestAnimationFrame(syncPosition);
    };
    syncPosition();
  }, [body]);

  const bind = useDrag(({ event, first, last, xy: [x, y] }) => {
    if (first) {
      isDragging.current = true;
      initialRect.current = containerRef.current.getBoundingClientRect();
      Matter.Body.setStatic(body, true);
      Matter.Body.setVelocity(body, { x: 0, y: 0 });
    }
    
    if (event) {
      const mouseX = x - initialRect.current.left;
      const mouseY = y - initialRect.current.top;
      
      Matter.Body.setPosition(body, { x: mouseX, y: mouseY });
      setDragPos({ x: mouseX, y: mouseY });
    }

    if (last) {
      isDragging.current = false;
      Matter.Body.setStatic(body, false);
      setDragPos(null);
    }
  }, {
    pointer: { buttons: 1 },
    filterTaps: true,
    delay: 0
  });

  const currentPos = dragPos || pos;

  return (
    <div
      {...bind()}
      style={{
        position: 'absolute',
        left: currentPos.x - 30,
        top: currentPos.y - 30,
        width: 60,
        height: 60,
        borderRadius: '50%',
        overflow: 'hidden',
        cursor: isDragging.current ? 'grabbing' : 'grab',
        transform: `translate3d(0,0,0) scale(${isDragging.current ? 1.1 : 1})`,
        transition: 'transform 0.15s',
        touchAction: 'none',
        userSelect: 'none',
        pointerEvents: 'auto'
      }}
    >
      <img 
        src={image} 
        alt="product" 
        draggable="false"
        style={{ 
          width: '100%', 
          height: '100%', 
          objectFit: 'cover',
          pointerEvents: 'none' 
        }} 
      />
    </div>
  );
};

export default PhysicsWorld;