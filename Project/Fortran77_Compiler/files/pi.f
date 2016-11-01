! Computes pi through numerical integration.
    
      program pi
      
      integer numrects, i
      real mid, height, width, area, sum
      
      sum = 0
      
      write(*, *) 'Number of rectangles:'
      read(*, *) numrects

      width = 1.0 / numrects
      do 42 i = numrects - 1, 0, -1
          mid = (i + 0.5) * width
          height = 4.0 / (1.0 + mid ** 2)
          sum = sum + height
42    continue      

      area = width * sum
      write(*, *) 'Computed pi = ', area       
            
      stop
      end
